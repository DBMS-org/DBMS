import { Component, EventEmitter, Output, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../core/models/user.model';

@Component({
  selector: 'app-operator-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class OperatorNavbarComponent implements OnInit, OnDestroy {
  @Output() toggleSidebar = new EventEmitter<void>();

  currentUser: User | null = null;
  private userSub: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userSub = this.authService.currentUser$.subscribe(u => this.currentUser = u);
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  getUserDisplayName(): string {
    return this.currentUser?.name || 'Operator';
  }

  navigateToProfile() {
    this.router.navigate(['/operator/profile']);
  }

  logout() {
    this.authService.logout();
  }
}
