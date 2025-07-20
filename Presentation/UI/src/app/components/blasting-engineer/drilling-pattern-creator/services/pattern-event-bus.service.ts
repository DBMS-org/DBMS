import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';

export type PatternCreatorEvent = {
  type: string;
  payload?: any;
};

@Injectable({
  providedIn: 'root'
})
export class PatternEventBusService {
  private eventBus$ = new Subject<PatternCreatorEvent>();

  emit(event: PatternCreatorEvent): void {
    this.eventBus$.next(event);
  }

  on<T>(eventType: string): Observable<T> {
    return this.eventBus$.pipe(
      filter(event => event.type === eventType),
      map(event => event.payload as T)
    );
  }
}