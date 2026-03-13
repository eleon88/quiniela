import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Board } from '../../boards/models/board';
import { Round, RoundStatus } from '../../rounds/models/round';
import { Match, MatchResult } from '../../rounds/models/match';
import { Participant } from '../models/participant';

export interface CreateRoundPayload {
  name: string;
  startDateTime: string;
  endDateTime: string;
}

export interface CreateMatchPayload {
  homeTeam: string;
  awayTeam: string;
  startDateTime: string;
}

@Injectable({ providedIn: 'root' })
export class AdminService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api`;

  getAdminBoards(): Observable<Board[]> {
    return this.http.get<Board[]>(`${this.apiUrl}/boards/admin`);
  }

  createRound(boardId: string, payload: CreateRoundPayload): Observable<Round> {
    return this.http.post<Round>(`${this.apiUrl}/boards/${boardId}/round`, payload);
  }

  createMatch(roundId: string, payload: CreateMatchPayload): Observable<Match> {
    return this.http.post<Match>(`${this.apiUrl}/round/${roundId}/matches`, payload);
  }

  updateRoundStatus(roundId: string, status: RoundStatus): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/round/${roundId}/status`, { status });
  }

  updateMatchResult(matchId: string, result: MatchResult): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/match/${matchId}/result`, { result });
  }

  getParticipants(roundId: string): Observable<Participant[]> {
    return this.http.get<Participant[]>(`${this.apiUrl}/round/${roundId}/participants`);
  }

  activateParticipant(participantId: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/participant/${participantId}/activate`, {});
  }
}
