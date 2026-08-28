import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild,
  Output,EventEmitter
} from '@angular/core';

import cytoscape, {
  Core,
  ElementDefinition
} from 'cytoscape';

@Component({
  selector: 'app-graph-view',
  standalone: true,
  imports: [],
  templateUrl: './graph-view.component.html',
  styleUrl: './graph-view.component.css'
})
export class GraphViewComponent
  implements AfterViewInit, OnChanges {

  @Input() graphData: any[] = [];
@Output() nodeSelected =  new EventEmitter<any>();
  @ViewChild('graph', { static: true })
  graphElement!: ElementRef;

  private cy?: Core;

  private viewInitialized = false;

  ngAfterViewInit(): void {

    this.viewInitialized = true;

    this.renderGraph();
  }

  ngOnChanges(changes: SimpleChanges): void {

    if (
      changes['graphData'] &&
      this.viewInitialized
    ) {
      this.renderGraph();
    }
  }

  private renderGraph(): void {

    if (!this.graphElement) {
      return;
    }

    if (this.cy) {
      this.cy.destroy();
    }

    const elements: ElementDefinition[] = [];

    const nodeIds = new Set<string>();

    const addNode = (
      id: string,
      label: string,
      type: string
    ): void => {

      if (nodeIds.has(id)) {
        return;
      }

      nodeIds.add(id);

      elements.push({
        data: {
          id,
          label,
          type
        }
      });
    };

    const addEdge = (
      id: string,
      source: string,
      target: string,
      label: string
    ): void => {

      elements.push({
        data: {
          id,
          source,
          target,
          label
        }
      });
    };

    for (const item of this.graphData) {

      const technology =
        item.technologyName;

      const relatedTechnology =
        item.relatedTechnology;

      const project =
        item.projectName;

      const domain =
        item.domainName;

      // Technology node

      if (technology) {

        addNode(
          `technology-${technology}`,
          technology,
          'technology'
        );
      }

      // Related Technology

      if (relatedTechnology) {

        addNode(
          `related-${relatedTechnology}`,
          relatedTechnology,
          'related'
        );

        addEdge(
          `related-${technology}-${relatedTechnology}`,
          `technology-${technology}`,
          `related-${relatedTechnology}`,
          'RELATED_TO'
        );
      }

      // Project

      if (project) {

        addNode(
          `project-${project}`,
          project,
          'project'
        );

        addEdge(
          `project-${project}-${technology}`,
          `project-${project}`,
          `technology-${technology}`,
          'USES'
        );
      }

      // Domain

      if (domain) {

        addNode(
          `domain-${domain}`,
          domain,
          'domain'
        );

        if (project) {

          addEdge(
            `domain-${project}-${domain}`,
            `project-${project}`,
            `domain-${domain}`,
            'IN_DOMAIN'
          );
        }
      }
    }

    // Create Cytoscape

    this.cy = cytoscape({

      container:
        this.graphElement.nativeElement,

      elements,

      style: [

        // Default node

        {
          selector: 'node',

          style: {
            'label': 'data(label)',
            'text-valign': 'center',
            'text-halign': 'center',
            'font-size': 12,
            'width': 80,
            'height': 80,
            'background-color': '#2563eb',
            'color': '#ffffff',
            'text-wrap': 'wrap',
            'text-max-width': '70'
          }
        },

        // Technology

        {
          selector:
            'node[type="technology"]',

          style: {
            'background-color': '#2563eb',
            'width': 90,
            'height': 90
          }
        },

        // Project

        {
          selector:
            'node[type="project"]',

          style: {
            'background-color': '#16a34a',
            'shape': 'round-rectangle',
            'width': 120,
            'height': 70
          }
        },

        // Domain

        {
          selector:
            'node[type="domain"]',

          style: {
            'background-color': '#9333ea',
            'shape': 'hexagon',
            'width': 90,
            'height': 70
          }
        },

        // Related Technology

        {
          selector:
            'node[type="related"]',

          style: {
            'background-color': '#ea580c',
            'width': 90,
            'height': 90
          }
        },

        // Edges

        {
          selector: 'edge',

          style: {
            'width': 2,
            'line-color': '#94a3b8',
            'target-arrow-color': '#94a3b8',
            'target-arrow-shape': 'triangle',
            'curve-style': 'bezier',
            'label': 'data(label)',
            'font-size': 9,
            'color': '#475569',
            'text-background-color': '#ffffff',
            'text-background-opacity': 1,
            'text-background-padding': '3'
          }
        }
      ],

      layout: {
        name: 'breadthfirst',
        directed: true,
        padding: 40,
        spacingFactor: 1.2
      }
    });
    this.cy.on('tap', 'node', (event) => {

  const node = event.target;

  this.nodeSelected.emit({
    id: node.id(),
    label: node.data('label'),
    type: node.data('type')
  });

});
  }
}