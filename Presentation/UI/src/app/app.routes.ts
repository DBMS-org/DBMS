import { Routes } from '@angular/router';
import {
  LoginComponent,
  ForgotPasswordComponent,
  VerifyResetCodeComponent,
  ResetPasswordComponent
} from './components/auth';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { ADMIN_ROUTES } from './components/admin/admin.routes';
import { BLASTING_ENGINEER_ROUTES } from './components/blasting-engineer/blasting-engineer.routes';
import { MECHANICAL_ENGINEER_ROUTES } from './components/mechanical-engineer/mechanical-engineer.routes';
import { MACHINE_MANAGER_ROUTES } from './components/machine-manager/machine-manager.routes';
import { OPERATOR_ROUTES } from './components/operator/operator.routes';
import { EXPLOSIVE_MANAGER_ROUTES } from './components/explosive-manager/explosive-manager.routes';
import { STORE_MANAGER_ROUTES } from './components/store-manager/store-manager.routes';

// Application route configuration with role-based access control
export const routes: Routes = [
  // Public authentication routes
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'verify-reset-code', component: VerifyResetCodeComponent },
  { path: 'reset-password', component: ResetPasswordComponent },

  // Protected admin module routes
  {
    path: 'admin',
    children: ADMIN_ROUTES,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' }
  },

  // Protected blasting engineer module routes
  {
    path: 'blasting-engineer',
    children: BLASTING_ENGINEER_ROUTES,
    canActivate: [authGuard, roleGuard],
    data: { role: 'BlastingEngineer' }
  },

  // Protected mechanical engineer module routes
  {
    path: 'mechanical-engineer',
    children: MECHANICAL_ENGINEER_ROUTES,
    canActivate: [authGuard, roleGuard],
    data: { role: 'MechanicalEngineer' }
  },

  // Protected machine manager module routes
  {
    path: 'machine-manager',
    children: MACHINE_MANAGER_ROUTES,
    canActivate: [authGuard, roleGuard],
    data: { role: 'MachineManager' }
  },

  // Protected operator module routes
  {
    path: 'operator',
    children: OPERATOR_ROUTES,
    canActivate: [authGuard, roleGuard],
    data: { role: 'Operator' }
  },

  // Protected explosive manager module routes
  {
    path: 'explosive-manager',
    children: EXPLOSIVE_MANAGER_ROUTES,
    canActivate: [authGuard, roleGuard],
    data: { role: 'ExplosiveManager' }
  },

  // Protected store manager module routes
  {
    path: 'store-manager',
    children: STORE_MANAGER_ROUTES,
    canActivate: [authGuard, roleGuard],
    data: { role: 'StoreManager' }
  },

  // Default route redirects to login page
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
