import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExplosiveRequest, ExplosiveType } from '../models/request.model';

@Component({
  selector: 'app-anfo-requests',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './anfo-requests.component.html',
  styleUrl: './anfo-requests.component.scss'
})
export class AnfoRequestsComponent implements OnChanges {
  @Input() requests: ExplosiveRequest[] = [];
  anfoRequests: ExplosiveRequest[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['requests'] && changes['requests'].currentValue) {
      this.anfoRequests = this.requests.filter(request => 
        request.explosiveType === ExplosiveType.ANFO
      );
    }
  }
}
