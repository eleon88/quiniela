import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Round } from '../models/round';

@Injectable({ providedIn: 'root' })
export class RoundsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api`;

  getRounds(boardId: string): Observable<Round[]> {
    return this.http.get<Round[]>(`${this.apiUrl}/rounds/${boardId}`);
  }
}
