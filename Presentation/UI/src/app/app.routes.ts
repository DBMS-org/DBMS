import { Routes } from '@angular/router';
import {
  LoginComponent,
  ForgotPasswordComponent,
  VerifyResetCodeComponent,
  ResetPasswordComponent
} from './components/auth';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

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
    loadChildren: () => import('./components/admin/admin.routes').then(m => m.ADMIN_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' }
  },

  // Protected blasting engineer module routes
  {
    path: 'blasting-engineer',
    loadChildren: () => import('./components/blasting-engineer/blasting-engineer.routes').then(m => m.BLASTING_ENGINEER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'BlastingEngineer' }
  },

  // Protected mechanical engineer module routes
  {
    path: 'mechanical-engineer',
    loadChildren: () => import('./components/mechanical-engineer/mechanical-engineer.routes').then(m => m.MECHANICAL_ENGINEER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'MechanicalEngineer' }
  },

  // Protected machine manager module routes
  {
    path: 'machine-manager',
    loadChildren: () => import('./components/machine-manager/machine-manager.routes').then(m => m.MACHINE_MANAGER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'MachineManager' }
  },

  // Protected operator module routes
  {
    path: 'operator',
    loadChildren: () => import('./components/operator/operator.routes').then(m => m.OPERATOR_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'Operator' }
  },

  // Protected explosive manager module routes
  {
    path: 'explosive-manager',
    loadChildren: () => import('./components/explosive-manager/explosive-manager.routes').then(m => m.EXPLOSIVE_MANAGER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'ExplosiveManager' }
  },

  // Protected store manager module routes
  {
    path: 'store-manager',
    loadChildren: () => import('./components/store-manager/store-manager.routes').then(m => m.STORE_MANAGER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'StoreManager' }
  },

  // Default route redirects to login page
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
