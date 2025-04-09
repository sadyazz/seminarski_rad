import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { MojConfig } from '../moj-config';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MyAuthService } from '../services/MyAuthService';
import { latLng, marker, tileLayer, icon, Map } from 'leaflet';
import { isPlatformBrowser } from '@angular/common';
import { jsPDF } from 'jspdf';


@Component({
  selector: 'property',
  templateUrl: './property.component.html',
  styleUrls: ['./property.component.css']
})
export class PropertyComponent implements OnInit {
  Images = {
    path: ''
  };
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
    reviews: [],
    latitude:0,
    longitude:0
  };
  
  amenities: any[] = [];
  checkinDate: Date | null = null;
  checkoutDate: Date | null = null;
  guests = 1;
  paymentMethods = 1;
  nights = 0;
  totalPrice = 0;
  pricePerNight = 0;
  numberOfReviews: number = 0;
  noReviews = true;

  propertyCoordinates: { [key: number]: { lat: number; lng: number } } = {
    2: { lat: 43.33682975700968, lng: 17.812479626353678 },
    3: { lat: 43.33865, lng: 17.81908 }, 
    4: { lat:43.33720714560264, lng:17.812708426353726 }, 
    5: { lat:43.34414, lng:17.80979 },
    6: { lat:43.33788, lng:17.81314 },
    7: { lat:43.34483, lng:17.81351 },
  };

  showDownloadPdfButton = false;

  constructor(
    public router: Router,
    private route: ActivatedRoute,
    private httpKlijent: HttpClient,
    private snackBar: MatSnackBar,
    private myAuthService: MyAuthService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}



  ngOnInit(): void {
    this.route.params.subscribe(x => {
      this.propertyId = <number>x['id'];
      this.loadPropertyDetails(this.propertyId);
      this.loadPropertyImages(this.propertyId);
    });
  }

  mapOptions: any;
  mapMarkers: any[] = [];
  map!: any;

  ngAfterViewInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      import('leaflet').then((leaflet) => {
        setTimeout(() => {
          this.initializeMap(leaflet);
          this.map.invalidateSize();
        }, 500);
      }).catch(error => {
        console.error('Error loading Leaflet:', error);
      });
    }
  }
  
  initializeMap(location: any): void {
    import('leaflet').then((leaflet) => {
      const { Map, tileLayer, icon, marker, latLng } = leaflet;
  
      this.map = new Map('map', {
        zoomControl: true,
      }).setView(location, 16);
      
      tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors',
      }).addTo(this.map);
    
      const redIcon = icon({
        iconUrl: 'assets/img/gps.png',
        iconSize: [32, 32],
      });
      
      marker(location, { icon: redIcon }).addTo(this.map);
    }).catch(error => {
      console.error('Error loading Leaflet:', error);
    });
  }
  
  getPropertyCoordinates(propertyId: number): void {
    const url = `${MojConfig.adresa_servera}/GetCoordinates?id=${propertyId}`;
    this.httpKlijent.get<any>(url).subscribe(
      data => {
        const lat = data.latitude;
        const lon = data.longitude;
        const location = latLng(lat, lon);
        this.initializeMap(location);
      },
      error => {
        console.error('Error fetching property coordinates', error);
      }
    );
  }
  
  addMarker(location: any, redIcon: any): void {
    this.mapMarkers.forEach((m: any) => this.map.removeLayer(m));
    this.mapMarkers = [];
  
    const newMarker = marker(location, { icon: redIcon }).addTo(this.map);
    this.mapMarkers.push(newMarker);
  }
  

  getTotalPrice() {
    return this.pricePerNight * this.nights * this.guests;
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

  loadPropertyDetails(id: number): void {
    const url = `${MojConfig.adresa_servera}/GetPropertyById?id=${id}`;
    this.httpKlijent.get<any>(url).subscribe(data => {
      this.property = data;
      this.property.images = data.images || [];
      this.pricePerNight = data.pricePerNight;
      this.property.reviews = data.reviews || [];
      this.numberOfReviews = this.property.reviews.length;
      this.noReviews = this.property.reviews.length === 0;
      this.loadAmenities(id);
      this.loadPropertyImages(id);

      const lat = data.latitude;
      const lon = data.longitude;
      const location = latLng(lat, lon);
      this.initializeMap(location);


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
      this.showDownloadPdfButton = true;
    }, error => {
      console.error('Error making reservation', error);
      this.snackBar.open('Reservation failed. Please try again.', 'Close', { duration: 3000 });
    });
  }

  nazad() {
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

  downloadPdf(): void {
    const doc = new jsPDF();
    
    const logoPath = 'assets/img/logo.png'; 
    doc.addImage(logoPath, 'PNG', 10, 10, 15, 15);

    doc.setFont('helvetica', 'normal');
    
    doc.setFontSize(22);
    doc.setFont('helvetica', 'bold');
    doc.text('eReservation', 150, 20);
    
    doc.setFontSize(16);
    doc.setFont('helvetica', 'normal');
    doc.text(this.property.name, 120, 30);
    
    doc.setLineWidth(0.5);
    doc.line(10, 35, 200, 35);
  
    doc.setFontSize(12);
    doc.setFont('helvetica', 'bold');
    doc.text('Property Details:', 10, 45);
    
    const propertyDetails = [
      ['Address:', this.property.address || 'N/A'],
      ['Rooms:', this.property.numberOfRooms.toString() || 'N/A'],
      ['Bathrooms:', this.property.numberOfBathrooms.toString() || 'N/A'],
      ['Price per Night:', `${this.pricePerNight} KM` || 'N/A'],
      ['Property Type:', this.property.propertyTypeName || 'N/A'],
      ['City:', this.property.cityName || 'N/A']
    ];
    
    let y = 55;
    propertyDetails.forEach(item => {
      doc.text(item[0], 10, y);
      doc.text(item[1], 90, y);
      y += 10;
    });
  

    doc.setFont('helvetica', 'bold');
    doc.text('Reservation Details:', 10, y + 10);
    
    const reservationDetails = [
      ['Check-in:', this.checkinDate?.toLocaleDateString()],
      ['Check-out:', this.checkoutDate?.toLocaleDateString()],
      ['Guests:', this.guests],
      ['Total Price:', `${this.totalPrice} KM`]
    ];
    
    y += 20;
    reservationDetails.forEach((item, index) => {
      const label = item[0] !== undefined ? String(item[0]) : 'N/A';
    
      doc.text(label, 10, y);
    
      const value = item[1] !== undefined && item[1] !== null ? String(item[1]) : 'N/A';
    
      doc.text(value, 90, y);
      y += 10;
    });
  

    doc.setFont('helvetica', 'bold');
    doc.text('Amenities:', 10, y + 10);
    

    let amenitiesStartY = y + 20;
    this.amenities.forEach((amenity, index) => {
      doc.setFont('helvetica', 'normal');
      doc.text(`- ${amenity.name}: ${amenity.description}`, 10, amenitiesStartY + index * 10);
    });
  

    doc.setLineWidth(0.5);
    doc.line(10, amenitiesStartY + (this.amenities.length * 10) + 10, 200, amenitiesStartY + (this.amenities.length * 10) + 10);
  

    doc.setFontSize(8);
    doc.setFont('helvetica', 'normal');
    doc.text('Thank you for choosing our property!', 10, amenitiesStartY + (this.amenities.length * 10) + 30);

    doc.save('reservation-bill.pdf');
  }
  
  uploadImage(event: any): void {

    const files = event.target.files;
    
    if (files && files[0]) {
      const file = files[0];
      const formData = new FormData();
      formData.append('image', file);
      formData.append('id', this.propertyId.toString());
  
      const url = `${MojConfig.adresa_servera}/UploadPropertyImage?id=${this.propertyId}`;
  
      this.httpKlijent.post(url, formData).subscribe(
        (response) => {
          this.snackBar.open('Image uploaded successfully!', 'Close', { duration: 3000 });
          this.loadPropertyImages(this.propertyId);
        },
        (error) => {
          console.error('Error uploading image', error);
          this.snackBar.open('Failed to upload image. Please try again.', 'Close', { duration: 3000 });
        }
      );
    } else {
      this.snackBar.open('No image selected', 'Close', { duration: 3000 });
    }
  }
  
  

}
