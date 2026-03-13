import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LeaderboardEntry } from '../models/leaderboard-entry';

@Injectable({ providedIn: 'root' })
export class LeaderboardService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api`;

  getLeaderboard(roundId: string): Observable<LeaderboardEntry[]> {
    return this.http.get<LeaderboardEntry[]>(`${this.apiUrl}/round/${roundId}/leaderboard`);
  }
}
