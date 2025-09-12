import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoreFiltersComponent } from './store-filters.component';

describe('StoreFiltersComponent', () => {
  let component: StoreFiltersComponent;
  let fixture: ComponentFixture<StoreFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StoreFiltersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StoreFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
