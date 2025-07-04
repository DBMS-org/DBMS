import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface AppState {
    activeProjectId: number | null;
    activeSiteId: number | null;
    isLoading: boolean;
}

@Injectable({
    providedIn: 'root'
})
export class StateService {
    private readonly state = new BehaviorSubject<AppState>({
        activeProjectId: null,
        activeSiteId: null,
        isLoading: false
    });

    public readonly state$ = this.state.asObservable();

    public get currentState(): AppState {
        return this.state.getValue();
    }

    public setProjectId(projectId: number | null): void {
        this.state.next({
            ...this.currentState,
            activeProjectId: projectId,
            activeSiteId: null // Reset site when project changes
        });
    }

    public setSiteId(siteId: number | null): void {
        this.state.next({
            ...this.currentState,
            activeSiteId: siteId
        });
    }

    public setLoading(isLoading: boolean): void {
        this.state.next({
            ...this.currentState,
            isLoading
        });
    }
} 