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
  templateUrl: './navbar.component.html'
})
export class NavbarComponent implements OnInit, OnDestroy {
  @Output() toggleSidebar = new EventEmitter<void>();
  
  currentUser: User | null = null;
  private userSubscription: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  ngOnDestroy() {
    this.userSubscription.unsubscribe();
  }

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  getUserDisplayName(): string {
    return this.currentUser?.name || 'Admin User';
  }

  navigateToProfile() {
    this.router.navigate(['/admin/profile']);
  }

  logout() {
    this.authService.logoutWithConfirmation();
  }
}
