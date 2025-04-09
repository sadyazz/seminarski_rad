import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MojConfig } from '../moj-config';
import { isPlatformBrowser } from '@angular/common';
import * as L from 'leaflet';

export interface CreatePropertyDto {
  name: string;
  address: string;
  numberOfRooms: number;
  numberOfBathrooms: number;
  pricePerNight: number;
  cityID: number;
  propertyTypeID: number;
  amenitiesIDs: number[];
  userID: number; 
  latitude: number;
  longitude:number;
}


interface City {
  id: number;
  name: string;
}

interface PropertyType {
  id: number;
  name: string;
}

interface Amenities {
  id: number;
  name: string;
}

interface User {
  id: number;
  name: string;
}

@Component({
  selector: 'app-add-property-dialog',
  templateUrl: './add-property-dialog.component.html',
  styleUrls: ['./add-property-dialog.component.css']
})
export class AddPropertyDialogComponent implements OnInit {
  propertyForm!: FormGroup;
  cities: City[] = [];
  propertyTypes: PropertyType[] = [];
  amenities: Amenities[] = [];
  users: User[] = [];
  selectedFile: File | null = null;

  map!: L.Map;
  marker!: L.Marker;

  constructor(
    private fb: FormBuilder,
    private httpClient: HttpClient,
    public dialogRef: MatDialogRef<AddPropertyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.propertyForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      numberOfRooms: [1, [Validators.required, Validators.min(1)]],
      numberOfBathrooms: [1, [Validators.required, Validators.min(1)]],
      pricePerNight: [0, [Validators.required, Validators.min(0)]],
      cityID: [null, Validators.required],
      propertyTypeID: [null, Validators.required],
      userID: [null, Validators.required],
      amenities: this.fb.array([]),
      latitude: [0, Validators.required],
      longitude: [0, Validators.required]
    });
  }

  ngOnInit(): void {

    this.httpClient.get<City[]>(`${MojConfig.adresa_servera}/api/City/GetAll`).subscribe(cities => {
      this.cities = cities;
    });

    this.httpClient.get<PropertyType[]>(`${MojConfig.adresa_servera}/api/PropertyType/GetAll`).subscribe(propertyTypes => {
      this.propertyTypes = propertyTypes;
    });

    this.httpClient.get<Amenities[]>(`${MojConfig.adresa_servera}/api/Amenities/GetAll`).subscribe(amenities => {
      this.amenities = amenities;

      const amenitiesFormArray = this.propertyForm.get('amenities') as FormArray;

      this.amenities.forEach(() => {
        amenitiesFormArray.push(this.fb.control(false));
      });
    });


    this.httpClient.get<User[]>(`${MojConfig.adresa_servera}/api/User/GetAll`).subscribe(users => {
      this.users = users;
    });
  }

  ngAfterViewInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.initMap();  // Initialize the map only in the browser
    }
  }

  initMap(): void {
    const initialLat = this.propertyForm.value.latitude;
    const initialLng = this.propertyForm.value.longitude;

    this.map = L.map('map').setView([initialLat, initialLng], 16);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors',
    }).addTo(this.map);

    this.marker = L.marker([initialLat, initialLng], { draggable: true }).addTo(this.map);

    this.marker.on('dragend', () => {
      const { lat, lng } = this.marker.getLatLng();
      this.propertyForm.patchValue({ latitude: lat, longitude: lng });
    });

    this.map.on('click', (e: L.LeafletMouseEvent) => {
      const { lat, lng } = e.latlng;
      this.marker.setLatLng(e.latlng);
      this.propertyForm.patchValue({ latitude: lat, longitude: lng });
    });
  }

  get amenitiesFormArray() {
    return (this.propertyForm.get('amenities') as FormArray);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  submit(): void {
    if (this.propertyForm.valid) {
      const selectedAmenities = this.amenitiesFormArray.controls
      .map((control, index) => control.value ? this.amenities[index].id : null)
      .filter((id): id is number => id !== null);

      const property: CreatePropertyDto = {
        name: this.propertyForm.value.name,
        address: this.propertyForm.value.address,
        numberOfRooms: this.propertyForm.value.numberOfRooms,
        numberOfBathrooms: this.propertyForm.value.numberOfBathrooms,
        pricePerNight: this.propertyForm.value.pricePerNight,
        cityID: this.propertyForm.value.cityID,
        propertyTypeID: this.propertyForm.value.propertyTypeID,
        amenitiesIDs: selectedAmenities,
        userID: this.propertyForm.value.userID,
        latitude: this.propertyForm.value.latitude,
        longitude: this.propertyForm.value.longitude
      };

      console.log(property);

      const headers = new HttpHeaders().set('Content-Type', 'application/json');

      this.httpClient.post(`${MojConfig.adresa_servera}/api/Properties/Add`, property)
        .subscribe(
          (response: any) => {
            console.log("Property added successfully", response);
            this.dialogRef.close(response); 
          },
          (error) => {
            console.error("Error adding property", error);
          }
        );
    }
  }

  onFileSelected(event: any): void {
    const files = event.target.files;
    if (files) {
      this.selectedFile = files;
    }
  }

 

}
