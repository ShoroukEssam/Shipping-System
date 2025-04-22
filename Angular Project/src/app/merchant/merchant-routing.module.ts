import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderFormComponent } from './order-form/order-form.component';
import { PermissionGuard } from '../shared/guard/permission.guard';
import { AuthGuard } from '../shared/guard/auth.guard';

const routes: Routes = [
  { path: 'order/:id', component: OrderFormComponent , canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Orders.Create'||'Permissions.Orders.Edit'}},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MerchantRoutingModule { }
