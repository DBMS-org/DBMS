import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExplosiveRequest, ExplosiveType } from '../models/request.model';

@Component({
  selector: 'app-emulsion-requests',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './emulsion-requests.component.html',
  styleUrl: './emulsion-requests.component.scss'
})
export class EmulsionRequestsComponent implements OnChanges {
  @Input() requests: ExplosiveRequest[] = [];
  emulsionRequests: ExplosiveRequest[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['requests'] && changes['requests'].currentValue) {
      this.emulsionRequests = this.requests.filter(req => req.explosiveType === ExplosiveType.EMULSION);
    }
  }



}
