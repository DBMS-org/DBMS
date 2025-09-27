import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApprovalFormComponent } from './approval-form.component';
import { RequestService } from '../services/request.service';
import { ExplosiveType } from '../../../../core/models/store.model';
import { of } from 'rxjs';

describe('ApprovalFormComponent', () => {
  let component: ApprovalFormComponent;
  let fixture: ComponentFixture<ApprovalFormComponent>;
  let requestService: jasmine.SpyObj<RequestService>;

  const mockRequest = {
    id: '1',
    requesterId: 'SM001',
    requesterName: 'John Smith',
    requesterRole: 'Store Manager',
    explosiveType: ExplosiveType.ANFO,
    quantity: 0.5,
    unit: 'tons',
    requestDate: new Date(),
    requiredDate: new Date(),
    status: 'PENDING',
    storeLocation: 'Warehouse A',
    purpose: 'Mining operation',
    notes: 'Urgent requirement'
  };

  beforeEach(async () => {
    const requestServiceSpy = jasmine.createSpyObj('RequestService', [
      'approveRequest',
      'rejectRequest',
      'setPending'
    ]);
    requestServiceSpy.approveRequest.and.returnValue(of(mockRequest));
    requestServiceSpy.rejectRequest.and.returnValue(of(mockRequest));
    requestServiceSpy.setPending.and.returnValue(of(mockRequest));

    await TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        ApprovalFormComponent
      ],
      providers: [
        { provide: RequestService, useValue: requestServiceSpy },
        { provide: MatDialogRef, useValue: { close: () => {} } },
        { provide: MAT_DIALOG_DATA, useValue: { request: mockRequest } }
      ]
    }).compileComponents();

    requestService = TestBed.inject(RequestService) as jasmine.SpyObj<RequestService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty values', () => {
    expect(component.approvalForm.get('decision')?.value).toBe('');
    expect(component.approvalForm.get('comments')?.value).toBe('');
  });

  it('should enable approval fields when decision is approve', () => {
    component.approvalForm.get('decision')?.setValue('approve');
    
    expect(component.approvalForm.get('approvedQuantity')?.enabled).toBeTrue();
    expect(component.approvalForm.get('departureDate')?.enabled).toBeTrue();
    expect(component.approvalForm.get('expectedReceiptDate')?.enabled).toBeTrue();
  });

  it('should enable rejection reason when decision is reject', () => {
    component.approvalForm.get('decision')?.setValue('reject');
    
    expect(component.approvalForm.get('rejectionReason')?.enabled).toBeTrue();
  });

  it('should validate dates correctly', () => {
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);

    component.approvalForm.patchValue({
      departureDate: today,
      expectedReceiptDate: tomorrow
    });

    expect(component.validateDates()).toBeTrue();
  });

  it('should call approveRequest service method when form is submitted with approve decision', () => {
    const approvalData = {
      decision: 'approve',
      approvedQuantity: 0.5,
      departureDate: new Date(),
      expectedReceiptDate: new Date(),
      comments: 'Approved with standard quantity'
    };

    component.approvalForm.patchValue(approvalData);
    component.onSubmit();

    expect(requestService.approveRequest).toHaveBeenCalled();
  });

  it('should call rejectRequest service method when form is submitted with reject decision', () => {
    const rejectionData = {
      decision: 'reject',
      rejectionReason: 'Insufficient safety documentation',
      comments: 'Please provide complete safety documentation'
    };

    component.approvalForm.patchValue(rejectionData);
    component.onSubmit();

    expect(requestService.rejectRequest).toHaveBeenCalled();
  });
});