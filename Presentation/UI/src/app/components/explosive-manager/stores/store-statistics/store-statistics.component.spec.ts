import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoreStatisticsComponent } from './store-statistics.component';

describe('StoreStatisticsComponent', () => {
  let component: StoreStatisticsComponent;
  let fixture: ComponentFixture<StoreStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StoreStatisticsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StoreStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
