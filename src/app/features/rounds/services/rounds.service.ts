import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Round } from '../models/round';
import { Match } from '../models/match';

@Injectable({ providedIn: 'root' })
export class RoundsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api`;

  getRounds(boardId: string): Observable<Round[]> {
    return this.http.get<Round[]>(`${this.apiUrl}/rounds/by-board/${boardId}`);
  }

  getRound(roundId: string): Observable<Round> {
    return this.http.get<Round>(`${this.apiUrl}/rounds/${roundId}`);
  }

  getMatches(roundId: string): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/rounds/${roundId}/matches`);
  }
}
