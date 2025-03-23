import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation-dialog',
  template: `
    <h2 mat-dialog-title>Are you sure you want to cancel this reservation?</h2>
    <mat-dialog-actions>
      <button mat-button (click)="onCancel()">No</button>
      <button mat-button (click)="onConfirm()">Yes</button>
    </mat-dialog-actions>
  `
})
export class ConfirmationDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<ConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any // Get the reservationId from parent component
  ) {}

  onCancel(): void {
    this.dialogRef.close(false);  // Close the dialog and send "false"
  }

  onConfirm(): void {
    this.dialogRef.close(true);  // Close the dialog and send "true"
  }
}
