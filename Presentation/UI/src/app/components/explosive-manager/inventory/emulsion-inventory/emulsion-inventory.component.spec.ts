import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmulsionInventoryComponent } from './emulsion-inventory.component';

describe('EmulsionInventoryComponent', () => {
  let component: EmulsionInventoryComponent;
  let fixture: ComponentFixture<EmulsionInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmulsionInventoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmulsionInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
