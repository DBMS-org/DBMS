import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { PatternData, BlastSequenceData } from '../drilling-pattern-creator/models/drill-point.model';

@Injectable({
  providedIn: 'root'
})
export class PatternDataService {
  private currentPatternSubject = new BehaviorSubject<PatternData | null>(null);
  private currentBlastSequenceSubject = new BehaviorSubject<BlastSequenceData | null>(null);

  constructor() { }

  // Pattern data management
  setCurrentPattern(patternData: PatternData): void {
    this.currentPatternSubject.next(patternData);
    console.log('Pattern data set:', patternData);
  }

  getCurrentPattern(): Observable<PatternData | null> {
    return this.currentPatternSubject.asObservable();
  }

  getCurrentPatternValue(): PatternData | null {
    return this.currentPatternSubject.value;
  }

  // Blast sequence data management
  setCurrentBlastSequence(blastSequenceData: BlastSequenceData): void {
    this.currentBlastSequenceSubject.next(blastSequenceData);
    console.log('Blast sequence data set:', blastSequenceData);
  }

  getCurrentBlastSequence(): Observable<BlastSequenceData | null> {
    return this.currentBlastSequenceSubject.asObservable();
  }

  getCurrentBlastSequenceValue(): BlastSequenceData | null {
    return this.currentBlastSequenceSubject.value;
  }

  // Clear data
  clearCurrentPattern(): void {
    this.currentPatternSubject.next(null);
  }

  clearCurrentBlastSequence(): void {
    this.currentBlastSequenceSubject.next(null);
  }

  // Check if pattern data is available
  hasPatternData(): boolean {
    return this.currentPatternSubject.value !== null;
  }

  // Export functionality
  exportBlastSequence(blastSequenceData: BlastSequenceData): void {
    const blob = new Blob([JSON.stringify(blastSequenceData, null, 2)], { type: 'application/json' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `blast-sequence-${new Date().toISOString().split('T')[0]}.json`;
    a.click();
    window.URL.revokeObjectURL(url);
  }

  // Import functionality
  importPattern(file: File): Promise<PatternData> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = (e) => {
        try {
          const patternData = JSON.parse(e.target?.result as string);
          this.setCurrentPattern(patternData);
          resolve(patternData);
        } catch (error) {
          reject(error);
        }
      };
      reader.onerror = () => reject(new Error('Failed to read file'));
      reader.readAsText(file);
    });
  }
} 