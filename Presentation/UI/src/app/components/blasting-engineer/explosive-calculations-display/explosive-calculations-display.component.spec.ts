import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExplosiveCalculationsDisplayComponent } from './explosive-calculations-display.component';

describe('ExplosiveCalculationsDisplayComponent', () => {
  let component: ExplosiveCalculationsDisplayComponent;
  let fixture: ComponentFixture<ExplosiveCalculationsDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExplosiveCalculationsDisplayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExplosiveCalculationsDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
