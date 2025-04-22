import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './environment';
import { IWeightSetting } from '../Models/Weight/weight';
import { AuthService } from './auth.service'; 

@Injectable({
  providedIn: 'root'
})
export class WeightSettingService {
  apiUrl: string = `${environment.baseUrl}/WeightSetting`;

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
    return headers;
  }

  getWeightSetting(): Observable<IWeightSetting> {
    const headers = this.getHeaders();
    return this.http.get<IWeightSetting>(this.apiUrl, { headers });
  }

  updateWeightSetting(ws: IWeightSetting): Observable<void> {
    const headers = this.getHeaders();
    return this.http.put<void>(this.apiUrl, ws, { headers });
  }
}
