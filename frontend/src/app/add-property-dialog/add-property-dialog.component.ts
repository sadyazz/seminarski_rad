import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { MojConfig } from '../moj-config';

export interface CreatePropertyDto {
  name: string;
  address: string;
  numberOfRooms: number;
  numberOfBathrooms: number;
  pricePerNight: number;
  cityID: number;
  propertyTypeID: number;
  amenitiesIDs: number[];
  userID: number;  // Add user ID to the request
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

  constructor(
    private fb: FormBuilder,
    private httpClient: HttpClient,
    public dialogRef: MatDialogRef<AddPropertyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
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
      };

      console.log(property);

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
