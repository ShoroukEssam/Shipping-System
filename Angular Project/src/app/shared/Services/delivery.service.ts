import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Delivery, IDelivery } from '../Models/Delivery/delivery';
import { environment } from './environment';
import { AuthService } from './auth.service'; // Adjust the path as necessary

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {

  private apiUrl = `${environment.baseUrl}/Delivery`;

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  getAllDeliveries(): Observable<IDelivery[]> {
    const headers = this.getHeaders();
    return this.http.get<IDelivery[]>(`${this.apiUrl}`, { headers });
  }

  getAllDeliveriesByState(state: string): Observable<Delivery[]> {
    const headers = this.getHeaders();
    return this.http.get<{ $id: string; $values: Delivery[] }>(`${this.apiUrl}`, { headers })
      .pipe(
        map(response => response.$values.filter((delegate: any) => delegate.government === state))
      );
  }

  getDeliveryById(id: string): Observable<IDelivery> {
    const headers = this.getHeaders();
    return this.http.get<IDelivery>(`${this.apiUrl}/${id}`, { headers });
  }

  addDelivery(delivery: Delivery): Observable<Delivery> {
    const headers = this.getHeaders();
    return this.http.post<Delivery>(`${this.apiUrl}/AddDelivery`, delivery, { headers });
  }

  updateDelivery(id: string, delivery: Delivery): Observable<Delivery> {
    const headers = this.getHeaders();
    return this.http.put<Delivery>(`${this.apiUrl}/EditDelivery/${id}`, delivery, { headers });
  }

  deleteDelivery(id: string): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete(`${this.apiUrl}/DeleteDelivery/${id}`, { headers });
  }

  changeStatus(id: string, status: boolean): Observable<any> {
    const headers = this.getHeaders();
    return this.http.put(`${this.apiUrl}/ChangeStatus/${id}`, { status }, { headers });
  }
}
