import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ParticipateRequest } from '../models/prediction';

@Injectable({ providedIn: 'root' })
export class PredictionsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api`;

  participate(roundId: string, request: ParticipateRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/round/${roundId}/participate`, request);
  }
}
