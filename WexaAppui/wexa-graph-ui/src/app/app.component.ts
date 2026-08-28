import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { GraphService } from './services/graph.service';
import { GraphViewComponent } from './components/graph-view/graph-view.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    FormsModule,
    GraphViewComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  technology = 'Angular';

  loading = false;
  searched = false;

  errorMessage = '';

  projects: string[] = [];

  domains: any[] = [];

  recommendations: any[] = [];

  graphData: any[] = [];

  selectedNode: any = null;

  constructor(
    private graphService: GraphService
  ) {}

  search(): void {

    if (!this.technology.trim()) {
      this.errorMessage = 'Please enter a technology.';
      return;
    }

    this.loading = true;
    this.searched = false;
    this.errorMessage = '';

    this.projects = [];
    this.domains = [];
    this.recommendations = [];
    this.graphData = [];
    this.selectedNode = null;

    const technology = this.technology.trim();

    this.graphService.getProjectsByTechnology(technology)
      .subscribe({
        next: (response) => {
          this.projects = response.projects ?? [];
        },
        error: () => {
          this.errorMessage = 'Failed to load projects.';
        }
      });

    this.graphService.getTechnologyDomains(technology)
      .subscribe({
        next: (response) => {
          this.domains = response.results ?? [];
        },
        error: () => {
          this.errorMessage = 'Failed to load domains.';
        }
      });

    this.graphService.getRecommendations(technology)
      .subscribe({
        next: (response) => {
          this.recommendations =
            response.recommendations ?? [];
        },
        error: () => {
          this.errorMessage =
            'Failed to load recommendations.';
        }
      });

    this.graphService.getGraph(technology)
      .subscribe({
        next: (response) => {

          this.graphData =
            response.graph ?? [];

          this.searched = true;
          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Graph API Error:',
            error
          );

          this.errorMessage =
            'Failed to load graph.';

          this.loading = false;
          this.searched = true;
        }
      });
  }

  onNodeSelected(node: any): void {

    this.selectedNode = node;

    console.log(
      'Selected Node:',
      node
    );
  }
}