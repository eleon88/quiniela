import { Component, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BoardsService } from '../../../boards/services/boards.service';
import { RoundsService } from '../../../rounds/services/rounds.service';
import { AdminService } from '../../services/admin.service';
import { RoundStatus } from '../../../rounds/models/round';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';
import { STATUS_LABEL, STATUS_CLASS } from '../../../../shared/constants/round-status';

@Component({
  selector: 'app-admin-board',
  imports: [
    RouterLink,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    LoadingComponent,
    ErrorMessageComponent,
  ],
  templateUrl: './admin-board.html',
  styleUrl: './admin-board.scss',
})
export class AdminBoardComponent {
  boardId = input.required<string>();

  private boardsService = inject(BoardsService);
  private roundsService = inject(RoundsService);
  private adminService = inject(AdminService);
  private fb = inject(FormBuilder);

  board = rxResource({
    params: () => this.boardId(),
    stream: ({ params: id }) => this.boardsService.getBoard(id),
  });

  rounds = rxResource({
    params: () => this.boardId(),
    stream: ({ params: boardId }) => this.roundsService.getRounds(boardId),
  });

  showCreateForm = signal(false);
  submitting = signal(false);
  createError = signal<string | null>(null);

  createForm = this.fb.nonNullable.group({
    name: ['', Validators.required],
    startDateTime: ['', Validators.required],
    endDateTime: ['', Validators.required],
  });

  readonly RoundStatus = RoundStatus;

  readonly statusLabel = STATUS_LABEL;
  readonly statusClass = STATUS_CLASS;

  toggleCreateForm(): void {
    this.showCreateForm.update(v => !v);
    if (!this.showCreateForm()) this.createForm.reset();
    this.createError.set(null);
  }

  submitCreateRound(): void {
    if (this.createForm.invalid || this.submitting()) return;
    this.submitting.set(true);
    this.createError.set(null);
    this.adminService.createRound(this.boardId(), this.createForm.getRawValue()).subscribe({
      next: () => {
        this.submitting.set(false);
        this.showCreateForm.set(false);
        this.createForm.reset();
        this.rounds.reload();
      },
      error: () => {
        this.submitting.set(false);
        this.createError.set('Failed to create round. Please try again.');
      },
    });
  }
}
