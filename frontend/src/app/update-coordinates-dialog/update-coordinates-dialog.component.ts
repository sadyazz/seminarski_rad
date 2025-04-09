import { Component, Inject, OnInit, AfterViewInit, PLATFORM_ID } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import * as L from 'leaflet';

@Component({
  selector: 'app-update-coordinates-dialog',
  templateUrl: './update-coordinates-dialog.component.html',
  styleUrls: ['./update-coordinates-dialog.component.css']
})
export class UpdateCoordinatesDialogComponent implements OnInit, AfterViewInit {
  latitude: number = 0;
  longitude: number = 0;
  private map: any;
  private marker: any;

  constructor(
    public dialogRef: MatDialogRef<UpdateCoordinatesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { latitude: number, longitude: number },
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.latitude = data.latitude;
    this.longitude = data.longitude;
  }

  ngOnInit(): void {
    if (this.data.latitude !== undefined && this.data.longitude !== undefined) {
      this.latitude = this.data.latitude;
      this.longitude = this.data.longitude;
    }
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  initMap(): void {
    this.map = L.map('map').setView([this.latitude, this.longitude], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(this.map);

    this.marker = L.marker([this.latitude, this.longitude], { draggable: true }).addTo(this.map);

    this.marker.on('dragend', () => {
      const position = this.marker.getLatLng();
      this.latitude = position.lat;
      this.longitude = position.lng;
    });
  }

  onSave(): void {
    this.dialogRef.close({ latitude: this.latitude, longitude: this.longitude });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
