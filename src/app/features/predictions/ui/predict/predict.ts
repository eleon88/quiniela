import { Component, computed, inject, input, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { RoundsService } from '../../../rounds/services/rounds.service';
import { PredictionsService } from '../../services/predictions.service';
import { SelectedOutcome } from '../../models/prediction';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';

@Component({
  selector: 'app-predict',
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
  templateUrl: './predict.html',
  styleUrl: './predict.scss',
})
export class PredictComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();

  private roundsService = inject(RoundsService);
  private predictionsService = inject(PredictionsService);
  private router = inject(Router);

  displayName = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required, Validators.maxLength(50), Validators.pattern(/^[\w\s\-'.]+$/)],
  });
  private displayNameValue = toSignal(this.displayName.valueChanges, { initialValue: '' });
  selections = signal<Map<string, SelectedOutcome>>(new Map());
  submitting = signal(false);
  submitError = signal('');

  round = rxResource({
    params: () => this.roundId(),
    stream: ({ params: id }) => this.roundsService.getRound(id),
  });

  matches = rxResource({
    params: () => this.roundId(),
    stream: ({ params: id }) => this.roundsService.getMatches(id),
  });

  allSelected = computed(() => {
    const matchList = this.matches.value();
    const sel = this.selections();
    return !!matchList?.length && sel.size === matchList.length;
  });

  canSubmit = computed(() =>
    this.displayNameValue().trim().length > 0 && this.allSelected() && !this.submitting(),
  );

  readonly SelectedOutcome = SelectedOutcome;

  selectionByMatch = computed(() => {
    const map: Record<string, SelectedOutcome> = {};
    for (const [id, outcome] of this.selections()) {
      map[id] = outcome;
    }
    return map;
  });

  selectOutcome(matchId: string, outcome: SelectedOutcome): void {
    this.selections.update(map => {
      const next = new Map(map);
      next.set(matchId, outcome);
      return next;
    });
  }

  submit(): void {
    if (!this.canSubmit()) return;

    this.submitting.set(true);
    this.submitError.set('');

    const predictions = Array.from(this.selections().entries()).map(
      ([matchId, selectedOutcome]) => ({ matchId, selectedOutcome }),
    );

    this.predictionsService
      .participate(this.roundId(), {
        displayName: this.displayName.value,
        predictions,
      })
      .subscribe({
        next: () => {
          this.router.navigate(['/boards', this.boardId(), 'rounds', this.roundId()]);
        },
        error: () => {
          this.submitError.set('Failed to submit prediction. Please try again.');
          this.submitting.set(false);
        },
      });
  }
}
