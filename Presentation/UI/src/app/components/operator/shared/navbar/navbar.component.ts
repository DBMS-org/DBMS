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
  // Emit event to toggle sidebar visibility
  @Output() toggleSidebar = new EventEmitter<void>();

  currentUser: User | null = null;
  private userSub: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  // Subscribe to current user changes
  ngOnInit(): void {
    this.userSub = this.authService.currentUser$.subscribe(u => this.currentUser = u);
  }

  // Clean up subscription
  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

  // Toggle sidebar on button click
  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  // Get user's display name or default
  getUserDisplayName(): string {
    return this.currentUser?.name || 'Operator';
  }

  // Navigate to user profile page
  navigateToProfile() {
    this.router.navigate(['/operator/profile']);
  }

  // Log out current user
  logout() {
    this.authService.logout();
  }
}
