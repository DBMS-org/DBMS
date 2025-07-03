import { Routes } from '@angular/router';
import { 
  LoginComponent, 
  ForgotPasswordComponent, 
  VerifyResetCodeComponent, 
  ResetPasswordComponent 
} from './components/auth';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'verify-reset-code', component: VerifyResetCodeComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { 
    path: 'admin',
    loadChildren: () => import('./components/admin/admin.routes').then(m => m.ADMIN_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'Admin' }
  },
  {
    path: 'blasting-engineer',
    loadChildren: () => import('./components/blasting-engineer/blasting-engineer.routes').then(m => m.BLASTING_ENGINEER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'BlastingEngineer' }
  },
  {
    path: 'mechanical-engineer',
    loadChildren: () => import('./components/mechanical-engineer/mechanical-engineer.routes').then(m => m.MECHANICAL_ENGINEER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'MechanicalEngineer' }
  },
  {
    path: 'machine-manager',
    loadChildren: () => import('./components/machine-manager/machine-manager.routes').then(m => m.MACHINE_MANAGER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'MachineManager' }
  },
  {
    path: 'operator',
    loadChildren: () => import('./components/operator/operator.routes').then(m => m.OPERATOR_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'Operator' }
  },
  {
    path: 'explosive-manager',
    loadChildren: () => import('./components/explosive-manager/explosive-manager.routes').then(m => m.EXPLOSIVE_MANAGER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'ExplosiveManager' }
  },
  {
    path: 'store-manager',
    loadChildren: () => import('./components/store-manager/store-manager.routes').then(m => m.STORE_MANAGER_ROUTES),
    canActivate: [authGuard, roleGuard],
    data: { role: 'StoreManager' }
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];
