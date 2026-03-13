import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UsersService {
  private http = inject(HttpClient);

  syncCurrentUser(email: string, displayName: string): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/api/users/me`, { email, displayName });
  }
}
