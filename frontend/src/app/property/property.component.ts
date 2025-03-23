import {Component, OnInit} from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from '@angular/router';
import {MojConfig} from "../moj-config";
import { MatSnackBar } from '@angular/material/snack-bar';
import { MyAuthService } from '../services/MyAuthService';

@Component({
  selector: 'property',
  templateUrl: './property.component.html',
  styleUrl: './property.component.css'
})
export class PropertyComponent implements OnInit{
  Images = {
    path:''
  }
  propertyId!: number;
  property: any = {
    name: '',
    adress: '',
    numberOfRooms: 0,
    numberOfBathrooms: 0,
    pricePerNight: 0,
    city: { name: '' },
    propertyType: { name: '' },
    images: [],
    reviews: []
  };


  amenities: any[] = [];
  checkinDate: Date | null = null;
  checkoutDate: Date | null = null;
  guests = 1;
  paymentMethods=1;
  nights = 0;
  totalPrice = 0;
  pricePerNight = 0;
  numberOfReviews: number = 0;
  noReviews= true;


  ngOnInit(): void {
    this.route.params.subscribe(x => {
      this.propertyId = <number>x['id']
      this.loadPropertyDetails(this.propertyId);
    });
  }
  getTotalPrice() {
    return (this.pricePerNight * this.nights* this.guests);
  }

  calculateNights() {
    if (this.checkinDate && this.checkoutDate) {
      const checkin = new Date(this.checkinDate);
      const checkout = new Date(this.checkoutDate);
      const timeDiff = checkout.getTime() - checkin.getTime();
      this.nights = timeDiff / (1000 * 3600 * 24);
    } else {
      this.nights = 0;
    }
  }
  onDateChange() {
    this.calculateNights();
    this.getTotalPrice();
  }



  constructor(public router: Router, private route: ActivatedRoute, private httpKlijent: HttpClient, private snackBar: MatSnackBar, private myAuthService: MyAuthService) {

  }

  loadPropertyDetails(id: number): void {
    const url = `${MojConfig.adresa_servera}/GetPropertyById?id=${id}`;
    this.httpKlijent.get<any>(url).subscribe(data => {
      this.property = data;
      this.property.images = data.images || [];
      this.pricePerNight=data.pricePerNight;
      this.property.reviews = data.reviews || [];
      this.numberOfReviews = this.property.reviews.length;
      if (this.property.reviews === null || this.property.reviews.length === 0) {
        this.noReviews = true;
      } else {
        this.noReviews = false;
      }
      this.loadAmenities(id);
      this.loadPropertyImages(id);
    }, error => {
      console.error('Error fetching property data', error);
    });
  }

  loadAmenities(propertyId: number): void {
    const url = `${MojConfig.adresa_servera}/api/Amenities/GetAmenitiesByPropertyId/${propertyId}`;
    this.httpKlijent.get<any[]>(url).subscribe(
      data => {
        this.amenities = data;
      },
      error => {
        console.error('Error fetching property amenities', error);
      }
    );
  }

  loadPropertyImages(propertyId: number): void {
    const url = `${MojConfig.adresa_servera}/GetPropertyImages?propertyId=${propertyId}`;
    this.httpKlijent.get<string[]>(url).subscribe(
      data => {
        this.property.images = data.map(imageBase64 => ({
          path: imageBase64
        }));
      },
      error => {
        console.error('Error fetching property images', error);
      }
    );
  }



  calculateNightsAndTotalPrice(): void {
    if (this.checkinDate && this.checkoutDate) {
      const timeDiff = Math.abs(this.checkoutDate.getTime() - this.checkinDate.getTime());
      this.nights = Math.ceil(timeDiff / (1000 * 3600 * 24));
      this.totalPrice = this.pricePerNight * this.nights;
    } else {
      this.nights = 0;
      this.totalPrice = 0;
    }
  }

  reserve(): void {
    const userId = this.myAuthService.returnId();

    if (!userId) {
      this.snackBar.open('You need to be logged in to make a reservation.', 'Close', { duration: 3000 });
      this.router.navigate(["/login"]);
      return;
    }

    const reservation = {
      dateOfArrival: this.checkinDate,
      dateOfDeparture: this.checkoutDate,
      status: "Pending",
      totalPrice: this.totalPrice,
      userId: userId,
      paymentMethodsId: this.paymentMethods,
      propertiesId: this.propertyId
    };

    const url = `${MojConfig.adresa_servera}/api/Reservations/Add`;
    this.httpKlijent.post(url, reservation).subscribe(response => {
      this.snackBar.open('Reservation successful!', 'Close', { duration: 3000 });
      this.router.navigate(["/home"]);
    }, error => {
      console.error('Error making reservation', error);
      this.snackBar.open('Reservation failed. Please try again.', 'Close', { duration: 3000 });
    });
  }


  nazad(){
    this.router.navigate(["/home"]);
  }


  review = {
    review: 1,
    comment: '',
    dateReview: new Date().toISOString(),
    userID: 0,
    propertiesID: 0
  };

  addReview() {
    const userId = this.myAuthService.returnId();
    if (!userId) {
      this.snackBar.open('You need to be logged in to leave a review.', 'Close', { duration: 3000 });
      this.router.navigate(["/login"]);
      return;
    }
  
    this.review.userID = userId;
    this.review.propertiesID = Number(this.propertyId);
    this.review.review = Number(this.review.review);
    this.review.dateReview = new Date().toISOString();

    console.log('Review payload before sending:', this.review);
  
    const url = `${MojConfig.adresa_servera}/api/Reviews/Add`;
    this.httpKlijent.post(url, this.review).subscribe(response => {
      this.snackBar.open('Review submitted successfully!', 'Close', { duration: 3000 });
      this.review = { review: 1, comment: '', dateReview: new Date().toISOString(), userID: 0, propertiesID: 0 };
      this.loadPropertyDetails(this.propertyId);
      window.location.reload();
    }, error => {
      console.error('Error submitting review', error);
      if (error.error && error.error.errors) {
        console.error('Validation Errors:', error.error.errors);
      }
      this.snackBar.open('Failed to submit review. Please try again.', 'Close', { duration: 3000 });
    });
  }


}
