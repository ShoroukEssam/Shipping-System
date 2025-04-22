import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LogInComponent } from './Components/log-in/log-in.component';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';
import { OrderListComponent } from './Components/order-list/order-list.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { OrderReportComponent } from './Components/order-report/order-report.component';
import { PermissionGuard } from './guard/permission.guard';
import { AuthGuard } from './guard/auth.guard';
const routes: Routes = [
  { path: 'home' , component: HomeComponent},
  { path: 'dashboard' , component: DashboardComponent, canActivate: [AuthGuard]},
  { path: 'login' , component: LogInComponent},
  { path: 'change-password' , component: ChangePasswordComponent,canActivate: [AuthGuard]},
  { path: 'order-list' , component: OrderListComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Orders.View'}},
  { path: 'order-list/:status' , component: OrderListComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Orders.View'}},
  { path: 'order-report/:id' , component: OrderReportComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Orders.View'}},
  { path: '**' , component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
exports: [RouterModule]
})
export class SharedRoutingModule { }
