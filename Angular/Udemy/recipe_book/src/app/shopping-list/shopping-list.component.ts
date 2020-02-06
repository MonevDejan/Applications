import { Component, OnInit } from '@angular/core';

import { Ingredient } from './../shared/ingredient.mode';
import { ShoppingListService } from './shopping-list.service';

import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.css']
})
export class ShoppingListComponent implements OnInit {

  ingredients: Ingredient[];
  durationInSeconds = 5;

  constructor(private shopingListService: ShoppingListService, private toastr: ToastrService) { }

  ngOnInit() {
    this.ingredients = this.shopingListService.getIngredients();
    this.shopingListService.ingredientsChange
      .subscribe(
        (ingredients: Ingredient[]) => {
          this.ingredients = ingredients;
        }
      )
  }

  showSuccess() {
    this.toastr.success('Hello world!', 'Toastr fun!');
  }

  // openSnackBar() {
  //   this._snackBar.open('Message archived')
  // }


}



