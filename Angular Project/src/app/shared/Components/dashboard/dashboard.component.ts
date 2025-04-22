import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderService } from './../../Services/order.service';
import { OrderStatus } from '../../Models/order/constants';
import { TranslationService } from '../../Services/translation.service';
import { Router } from '@angular/router';
import { AuthService } from './../../Services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit , OnDestroy{
  orderStatus = Object.values(OrderStatus);
  statusCounts: { status: string, count: number }[] = [];
  userRole: string='';
  OrderSubscription: any;
  constructor(
    private orderService: OrderService,
    private translationService: TranslationService,
    private router: Router,
    private authService: AuthService
  ) {}
  ngOnDestroy(): void {
    if (this.OrderSubscription) {
      this.OrderSubscription.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.userRole = this.authService.getUserData()?.role || '';
    this.getOrderStatusCounts();
  }

  getOrderStatusCounts(): void {
    this.orderStatus.forEach(status => {
      if (this.userRole === 'المناديب' && status === 'جديد') {
        return;
      }
      this.OrderSubscription = this.orderService.getOrdersByStatusCount(status).subscribe({
        next: (count: number) => {
          this.statusCounts.push({ status, count });
        },
        error: (error: any) => {
          this.statusCounts.push({ status, count: 0 });
        }
      });
    });
  }

  navigateToOrders(status: string): void {
    let translationRoute = this.translationService.translateToEnglish(status);
    this.router.navigate(['/employee/order-list', translationRoute]);
  }
}
