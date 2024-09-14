import {Component, Input, OnInit} from '@angular/core';
import {MojConfig} from "../../moj-config";
import {MyAuthService} from "../../services/MyAuthService";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'review-component',
  templateUrl: './review-component.component.html',
  styleUrl: './review-component.component.css'
})
export class ReviewComponentComponent implements OnInit{

  @Input() type: 'user' | 'property' = 'user';
  @Input() id: number = 0;
  reviews: any[] = [];
  noReviews: boolean = false;

  constructor(public myAuthService: MyAuthService, private httpClient: HttpClient) {
  }

  ngOnInit(): void {
    console.log('ReviewComponent initialized with ID:', this.id);
    this.getReviews();
  }

  getReviews(): void {
    let url: string;

    if (this.type === 'user') {
      const userId = this.myAuthService.returnId();
      url = `${MojConfig.adresa_servera}/GetReviewByUserId?userId=${userId}`;
    } else if (this.type === 'property') {
      url = `${MojConfig.adresa_servera}/GetReviewsByPropertyId?propertyId=${this.id}`;
    } else {
      console.error('Invalid review type');
      return;
    }

    this.httpClient.get<any[]>(url).subscribe(
      (data) => {
        console.log('Fetched reviews:', data);
        if (data === null || data.length === 0) {
          this.noReviews = true;
        } else {
          this.reviews = data;
          this.noReviews = false;
        }
      },
      (error) => {
        console.error('Error fetching reviews:', error);
      }
    );
  }
}
