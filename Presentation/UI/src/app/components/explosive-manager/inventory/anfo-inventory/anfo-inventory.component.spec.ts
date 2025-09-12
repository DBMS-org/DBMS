import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnfoInventoryComponent } from './anfo-inventory.component';

describe('AnfoInventoryComponent', () => {
  let component: AnfoInventoryComponent;
  let fixture: ComponentFixture<AnfoInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnfoInventoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AnfoInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
