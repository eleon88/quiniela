import { Component, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RoundsService } from '../../../rounds/services/rounds.service';
import { AdminService } from '../../services/admin.service';
import { RoundStatus } from '../../../rounds/models/round';
import { MatchResult } from '../../../rounds/models/match';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';
import { STATUS_LABEL, STATUS_CLASS, RESULT_LABEL } from '../../../../shared/constants/round-status';

@Component({
  selector: 'app-admin-round',
  imports: [
    RouterLink,
    ReactiveFormsModule,
    DatePipe,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    LoadingComponent,
    ErrorMessageComponent,
  ],
  templateUrl: './admin-round.html',
  styleUrl: './admin-round.scss',
})
export class AdminRoundComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();

  private roundsService = inject(RoundsService);
  private adminService = inject(AdminService);
  private fb = inject(FormBuilder);

  round = rxResource({
    params: () => this.roundId(),
    stream: ({ params: roundId }) => this.roundsService.getRound(roundId),
  });

  matches = rxResource({
    params: () => this.roundId(),
    stream: ({ params: roundId }) => this.roundsService.getMatches(roundId),
  });

  participants = rxResource({
    params: () => this.roundId(),
    stream: ({ params: roundId }) => this.adminService.getParticipants(roundId),
  });

  showAddMatchForm = signal(false);
  submittingMatch = signal(false);
  addMatchError = signal<string | null>(null);

  addMatchForm = this.fb.nonNullable.group({
    homeTeam: ['', Validators.required],
    awayTeam: ['', Validators.required],
    startDateTime: ['', Validators.required],
  });

  updatingStatus = signal(false);
  updatingMatchId = signal<string | null>(null);
  activatingParticipantId = signal<string | null>(null);

  readonly RoundStatus = RoundStatus;
  readonly MatchResult = MatchResult;

  readonly statusLabel = STATUS_LABEL;
  readonly statusClass = STATUS_CLASS;
  readonly resultLabel = RESULT_LABEL;

  private readonly nextStatusMap: Partial<Record<RoundStatus, RoundStatus>> = {
    [RoundStatus.Draft]: RoundStatus.Open,
    [RoundStatus.Open]: RoundStatus.Active,
    [RoundStatus.Active]: RoundStatus.Completed,
  };

  nextStatus(current: RoundStatus): RoundStatus | null {
    return this.nextStatusMap[current] ?? null;
  }

  advanceStatus(): void {
    const r = this.round.value();
    if (!r || this.updatingStatus()) return;
    const next = this.nextStatus(r.status);
    if (next === null) return;
    this.updatingStatus.set(true);
    this.adminService.updateRoundStatus(this.roundId(), next).subscribe({
      next: () => {
        this.updatingStatus.set(false);
        this.round.reload();
      },
      error: () => this.updatingStatus.set(false),
    });
  }

  setMatchResult(matchId: string, result: MatchResult): void {
    if (this.updatingMatchId()) return;
    this.updatingMatchId.set(matchId);
    this.adminService.updateMatchResult(matchId, result).subscribe({
      next: () => {
        this.updatingMatchId.set(null);
        this.matches.reload();
      },
      error: () => this.updatingMatchId.set(null),
    });
  }

  toggleAddMatchForm(): void {
    this.showAddMatchForm.update(v => !v);
    if (!this.showAddMatchForm()) this.addMatchForm.reset();
    this.addMatchError.set(null);
  }

  submitAddMatch(): void {
    if (this.addMatchForm.invalid || this.submittingMatch()) return;
    this.submittingMatch.set(true);
    this.addMatchError.set(null);
    this.adminService.createMatch(this.roundId(), this.addMatchForm.getRawValue()).subscribe({
      next: () => {
        this.submittingMatch.set(false);
        this.showAddMatchForm.set(false);
        this.addMatchForm.reset();
        this.matches.reload();
      },
      error: () => {
        this.submittingMatch.set(false);
        this.addMatchError.set('Failed to add match. Please try again.');
      },
    });
  }

  activateParticipant(participantId: string): void {
    if (this.activatingParticipantId()) return;
    this.activatingParticipantId.set(participantId);
    this.adminService.activateParticipant(participantId).subscribe({
      next: () => {
        this.activatingParticipantId.set(null);
        this.participants.reload();
      },
      error: () => this.activatingParticipantId.set(null),
    });
  }
}
