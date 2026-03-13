import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Board } from '../models/board';

@Injectable({ providedIn: 'root' })
export class BoardsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/boards`;

  getBoards(): Observable<Board[]> {
    return this.http.get<Board[]>(this.apiUrl);
  }

  getBoard(id: string): Observable<Board> {
    return this.http.get<Board>(`${this.apiUrl}/${id}`);
  }
}
