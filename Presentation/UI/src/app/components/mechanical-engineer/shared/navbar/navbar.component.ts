import { Component, EventEmitter, Output, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../core/models/user.model';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit, OnDestroy {
  @Output() toggleSidebar = new EventEmitter<void>();

  currentUser: User | null = null;
  private userSubscription: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  // Subscribe to current user data
  ngOnInit() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  // Clean up subscription
  ngOnDestroy() {
    this.userSubscription.unsubscribe();
  }

  // Emit sidebar toggle event
  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  // Get user name or default text
  getUserDisplayName(): string {
    return this.currentUser?.name || 'Mechanical Engineer';
  }

  // Navigate to profile page
  navigateToProfile() {
    this.router.navigate(['/mechanical-engineer/profile']);
  }

  // Log out current user
  logout() {
    this.authService.logout();
  }
}
