import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  product: IProduct;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    this.bcService.set('@productDetail', ' ');
    this.shopService.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(
      response => {
        this.product = response;
        this.bcService.set('@productDetail', this.product.name);
        console.log(this.product);

      }, error => {
        console.log(error);

      }
    )
  }
}
