import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ArticlesComponent } from './articles/articles.component';
import { AppComponent } from './app.component';

import { FeedConfigurationComponent } from './feed-configuration/feed-configuration.component';
import { ManageNewsComponent } from './manage-news/manage-news.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SingleArticleComponent } from './single-article/single-article.component';

const routes: Routes = [
  { path: '', redirectTo: '/SignIn', pathMatch: 'full' },
  { path: 'articles', component: ArticlesComponent },
  { path: 'articles/:id', component: SingleArticleComponent },
  { path: 'SignIn', component: SignInComponent },
  { path: 'manage-news', component: ManageNewsComponent },
  { path: 'feed-configuration', component: FeedConfigurationComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
