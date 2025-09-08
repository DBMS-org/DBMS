import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DrillDataService {
  private drillData: any[] = [];

  getDrillData(): any[] {
    return this.drillData;
  }

  setDrillData(data: any[]): void {
    this.drillData = [...data];
  }

  clearDrillData(): void {
    this.drillData = [];
  }
}










