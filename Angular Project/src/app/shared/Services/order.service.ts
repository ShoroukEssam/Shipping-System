import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { IorderResponse } from '../Models/order/orderResponse';
import { IOrder } from '../Models/order/order';
import { city } from '../Models/city';
import { Branch } from '../Models/branch';
import { OrderStatus } from '../Models/order/constants';
import { environment } from './environment';
import { AuthService } from './auth.service'; 
@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = `${environment.baseUrl}/Order`;

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    let token = this.authService.getUserData()?.token || '';
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }

  getAllOrders(): Observable<IorderResponse[]> {
    const headers = this.getHeaders();
    return this.http.get<IorderResponse[]>(`${this.apiUrl}/Index`, { headers });
  }

  getOrdersByStatus(status: OrderStatus | null): Observable<IOrder[]> {
    const headers = this.getHeaders();
    const statusParam = status ? encodeURIComponent(status) : '';
    return this.http.get<{ $values: IOrder[] }>(`${this.apiUrl}/GetOrdersDependonStatus?status=${statusParam}`, { headers })
      .pipe(
        map(response => response.$values)
      );
  }

  getOrdersByStatusCount(status: OrderStatus | null): Observable<number> {
    return this.getOrdersByStatus(status).pipe(
      map(orders => orders.length)
    );
  }

  searchOrdersByClientName(query: string): Observable<IorderResponse[]> {
    const headers = this.getHeaders();
    return this.http.get<IorderResponse[]>(`${this.apiUrl}/SearchByClientName?query=${query}`, { headers });
  }

  searchOrdersByDeliveryName(query: string): Observable<IorderResponse[]> {
    const headers = this.getHeaders();
    return this.http.get<IorderResponse[]>(`${this.apiUrl}/SearchByDeliveryName?query=${query}`, { headers });
  }

  getOrderReceipt(id: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get<any>(`${this.apiUrl}/OrderReceipt?id=${id}`, { headers });
  }

  changeOrderDelivery(id: number, deliveryId: number): Observable<void> {
    const headers = this.getHeaders();
    return this.http.put<void>(`${this.apiUrl}/ChangeDelivery?id=${id}&deliveryId=${deliveryId}`, {}, { headers });
  }

  changeOrderStatus(id: number, status: string): Observable<void> {
    const headers = this.getHeaders();
    return this.http.put<void>(`${this.apiUrl}/ChangeStatus?id=${id}&status=${status}`, {}, { headers });
  }

  editOrder(id: number, order: any): Observable<void> {
    const headers = this.getHeaders();
    return this.http.put<void>(`${this.apiUrl}/Edit/${id}`, order, { headers });
  }

  addOrder(order: IOrder): Observable<IorderResponse> {
    const headers = this.getHeaders();
    return this.http.post<IorderResponse>(`${this.apiUrl}/Add`, order, { headers });
  }

  deleteOrder(id: number): Observable<void> {
    const headers = this.getHeaders();
    return this.http.delete<void>(`${this.apiUrl}/Delete?id=${id}`, { headers });
  }

  getOrderCount(): Observable<IorderResponse[]> {
    const headers = this.getHeaders();
    return this.http.get<IorderResponse[]>(`${this.apiUrl}/OrderCount`, { headers });
  }

  getOrdersAfterFilter(query: string): Observable<IorderResponse[]> {
    const headers = this.getHeaders();
    return this.http.get<IorderResponse[]>(`${this.apiUrl}/IndexAfterFilter?query=${query}`, { headers });
  }

  getCitiesByGovernment(governmentId: number): Observable<city[]> {
    const headers = this.getHeaders();
    return this.http.get<city[]>(`${this.apiUrl}/GetCitiesByGovernment?governmentId=${governmentId}`, { headers });
  }

  getBranchesByGovernment(governmentId: number): Observable<Branch[]> {
    const headers = this.getHeaders();
    return this.http.get<Branch[]>(`${this.apiUrl}/GetBranchesByGovernment?government=${governmentId}`, { headers });
  }
}
