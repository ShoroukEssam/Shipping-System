import { SpinnerComponent } from './Components/spinner/spinner.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './Components/home/home.component';
import { LogInComponent } from './Components/log-in/log-in.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { OrderListComponent } from './Components/order-list/order-list.component';
import { AsideComponent } from './Components/aside/aside.component';
import { OrderReportComponent } from './Components/order-report/order-report.component';
import { SharedRoutingModule } from './shared-routing.module';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PasswordService } from './Services/password.service';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [
    HomeComponent,
    LogInComponent,
    DashboardComponent,
    ChangePasswordComponent,
    OrderListComponent,
    OrderReportComponent,
    AsideComponent,
    SpinnerComponent
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
  ],
  exports: [
    HomeComponent,
    LogInComponent,
    DashboardComponent,
    ChangePasswordComponent,
    OrderListComponent,
    OrderReportComponent,
    AsideComponent,
    ReactiveFormsModule,
    SpinnerComponent
  ],
  providers: [PasswordService]
})
export class SharedModule { }
