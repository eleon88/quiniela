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

  roundVersion = signal(0);
  matchesVersion = signal(0);
  participantsVersion = signal(0);

  round = rxResource({
    params: () => ({ boardId: this.boardId(), roundId: this.roundId(), v: this.roundVersion() }),
    stream: ({ params }) => this.roundsService.getRound(params.boardId, params.roundId),
  });

  matches = rxResource({
    params: () => ({ roundId: this.roundId(), v: this.matchesVersion() }),
    stream: ({ params }) => this.roundsService.getMatches(params.roundId),
  });

  participants = rxResource({
    params: () => ({ roundId: this.roundId(), v: this.participantsVersion() }),
    stream: ({ params }) => this.adminService.getParticipants(params.roundId),
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

  readonly statusLabel: Record<RoundStatus, string> = {
    [RoundStatus.Draft]: 'Draft',
    [RoundStatus.Open]: 'Open',
    [RoundStatus.Active]: 'Active',
    [RoundStatus.Completed]: 'Completed',
  };

  readonly statusClass: Record<RoundStatus, string> = {
    [RoundStatus.Draft]: 'status-draft',
    [RoundStatus.Open]: 'status-open',
    [RoundStatus.Active]: 'status-active',
    [RoundStatus.Completed]: 'status-completed',
  };

  readonly resultLabel: Record<MatchResult, string> = {
    [MatchResult.Pending]: 'Pending',
    [MatchResult.HomeWin]: 'Home Win',
    [MatchResult.AwayWin]: 'Away Win',
    [MatchResult.Draw]: 'Draw',
  };

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
        this.roundVersion.update(v => v + 1);
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
        this.matchesVersion.update(v => v + 1);
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
        this.matchesVersion.update(v => v + 1);
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
        this.participantsVersion.update(v => v + 1);
      },
      error: () => this.activatingParticipantId.set(null),
    });
  }
}
