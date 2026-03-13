import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Round } from '../models/round';
import { Match } from '../models/match';

@Injectable({ providedIn: 'root' })
export class RoundsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api`;

  getRounds(boardId: string): Observable<Round[]> {
    return this.http.get<Round[]>(`${this.apiUrl}/rounds/${boardId}`);
  }

  getRound(boardId: string, roundId: string): Observable<Round | undefined> {
    return this.getRounds(boardId).pipe(
      map(rounds => rounds.find(r => r.id === roundId)),
    );
  }

  getMatches(roundId: string): Observable<Match[]> {
    return this.http.get<Match[]>(`${this.apiUrl}/round/${roundId}/matches`);
  }
}
