import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from './environment';
import { IMerchantDTO } from '../Models/IMerchant';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class MerchantService {
  baseUrl: string = `${environment.baseUrl}/Merchant`;

  constructor(private http: HttpClient, private router: Router, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  getAllMerchants(): Observable<IMerchantDTO[]> {
    const headers = this.getHeaders();
    return this.http.get<IMerchantDTO[]>(`${this.baseUrl}`, { headers });
  }

  addMerchant(newMerchant: IMerchantDTO): Observable<IMerchantDTO> {
    const headers = this.getHeaders();
    return this.http.post<IMerchantDTO>(`${this.baseUrl}/AddMerchant`, newMerchant, { headers });
  }

  getMerchantById(id: string): Observable<IMerchantDTO> {
    const headers = this.getHeaders();
    return this.http.get<IMerchantDTO>(`${this.baseUrl}/GetMerchantById/${id}`, { headers });
  }

  updateMerchant(newData: IMerchantDTO): Observable<IMerchantDTO> {
    const headers = this.getHeaders();
    return this.http.put<IMerchantDTO>(`${this.baseUrl}/UpdateMerchant`, newData, { headers });
  }

  updateStatus(id: string, status: boolean): Observable<IMerchantDTO> {
    const headers = this.getHeaders();
    return this.http.put<IMerchantDTO>(`${this.baseUrl}/UpdateStatus`, { id, status }, { headers });
  }

  deleteMerchant(id: string): Observable<void> {
    const headers = this.getHeaders();
    return this.http.delete<void>(`${this.baseUrl}/DeleteMerchant/${id}`, { headers });
  }
}
