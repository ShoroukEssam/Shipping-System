import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BranchListComponent } from './Branches/branch-list/branch-list.component';
import { BranchFormComponent } from './Branches/branch-form/branch-form.component';
import { CityListComponent } from './Cities/city-list/city-list.component';
import { CityFormComponent } from './Cities/city-form/city-form.component';
import { WightSettingComponent } from './Weight/wight-setting/wight-setting.component';
import { StateListComponent } from './States/state-list/state-list.component';
import { StateFormComponent } from './States/state-form/state-form.component';
import { DeliveryListComponent } from './deliveries/delivery-list/delivery-list.component';
import { DeliveryFormComponent } from './deliveries/delivery-form/delivery-form.component';
import { MerchantListComponent } from './merchants/merchant-list/merchant-list.component';
import { MerchantFormComponent } from './merchants/merchant-form/merchant-form.component';
import { PermissionListComponent } from './Permissions/permission-list/permission-list.component';
import { UpdatePermissionComponent } from './Permissions/update-permission/update-permission.component';
import { AddPermissionComponent } from './Permissions/add-permission/add-permission.component';
import { EmployeeListComponent } from './employees/employee-list/employee-list.component';
import { EmployeeFormComponent } from './employees/employee-form/employee-form.component';
import { AuthGuard } from '../shared/guard/auth.guard';
import { PermissionGuard } from '../shared/guard/permission.guard';

const routes: Routes = [
  { path: 'branch', component: BranchListComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Branches.View'}},
  { path: 'branch/:id', component: BranchFormComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission:'Permissions.Branches.Create' ||'Permissions.Branches.Edit'}},
  // ----------------------------------------------------------- //
  { path: 'city/:id', component: CityListComponent ,canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Cities.View'}},
  { path: 'city/:state/:id', component: CityFormComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission:'Permissions.Cities.Create' ||'Permissions.Cities.Edit'}},
  // ----------------------------------------------------------- //
  { path: 'state', component: StateListComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Governments.View'}},
  { path: 'state/:id', component: StateFormComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission:'Permissions.Governments.Create' ||'Permissions.Governments.Edit'}},
  // ----------------------------------------------------------- //
  { path: 'delivery', component: DeliveryListComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Deliveries.View'}},
  { path: 'delivery/:id', component: DeliveryFormComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission:'Permissions.Deliveries.Create' ||'Permissions.Deliveries.Edit'}},
  // ----------------------------------------------------------- //
  { path: 'merchant', component: MerchantListComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Merchants.View'}},
  { path: 'merchant/:id', component: MerchantFormComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission:'Permissions.Merchants.Create' ||'Permissions.Merchants.Edit'}},
  // ----------------------------------------------------------- //
  { path: 'employee', component: EmployeeListComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Employees.View'}},
  { path: 'employee/new', component: EmployeeFormComponent ,canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Employees.Create'}},
  { path: 'employee/:id', component: EmployeeFormComponent ,canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Employees.Edit'||'Permissions.Employees.Create'}},
  // ----------------------------------------------------------- //
  { path: 'permission', component: PermissionListComponent ,canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Controls.View'}},
  { path: 'permission/:id', component: UpdatePermissionComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Controls.Edit'}},
  { path: 'permission/form/:id', component: AddPermissionComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.Controls.Create'||'Permissions.Controls.Edit'}},
  // ----------------------------------------------------------- //
  { path: 'weight', component: WightSettingComponent, canActivate: [AuthGuard,PermissionGuard], data: {permission: 'Permissions.WeightSettings.Edit'||'Permissions.WeightSettings.View'}},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EmployeeRoutingModule {}
