import { Injectable, ElementRef } from '@angular/core';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import html2canvas from 'html2canvas';
import { PatternData, BlastConnection } from '../../drilling-pattern-creator/models/drill-point.model';
import { SimulationSettings, SimulationState } from '../../shared/models/simulation.model';

@Injectable({ providedIn: 'root' })
export class ReportExportService {
  async exportReport({
    patternData,
    connections,
    simulationSettings,
    simulationState,
    projectName,
    canvasRef,
    metrics
  }: {
    patternData: PatternData | null;
    connections: BlastConnection[];
    simulationSettings: SimulationSettings;
    simulationState: SimulationState;
    projectName: string;
    canvasRef: ElementRef<HTMLDivElement>;
    metrics: { [key: string]: any };
  }): Promise<void> {
    const doc = new jsPDF({ orientation: 'landscape', unit: 'pt', format: 'a4' });
    let y = 40;
    doc.setFillColor(44, 62, 80);
    doc.rect(0, 0, doc.internal.pageSize.getWidth(), 60, 'F');
    doc.setTextColor(255, 255, 255);
    doc.setFontSize(22);
    doc.text('Blast Sequence Simulation Report', 40, 40);
    doc.setFontSize(12);
    doc.text(`Exported: ${new Date().toLocaleString()}`, doc.internal.pageSize.getWidth() - 200, 40);
    y = 70;
    doc.setTextColor(44, 62, 80);
    doc.setFillColor(236, 240, 241);
    doc.roundedRect(30, y, doc.internal.pageSize.getWidth() - 60, 50, 8, 8, 'F');
    doc.setFontSize(13);
    doc.text(`Project: ${projectName || 'N/A'}`, 50, y + 20);
    y += 70;
    // Pattern Data Section
    doc.setFontSize(15);
    doc.setTextColor(52, 73, 94);
    doc.text('Pattern Data', 40, y);
    doc.setDrawColor(52, 73, 94);
    doc.line(40, y + 3, 200, y + 3);
    y += 15;
    if (patternData) {
      doc.text(`Spacing: ${patternData.settings.spacing}`, 50, y);
      doc.text(`Burden: ${patternData.settings.burden}`, 200, y);
      doc.text(`Depth: ${patternData.settings.depth}`, 350, y);
      y += 10;
      autoTable(doc, {
        startY: y,
        head: [['ID', 'X (m)', 'Y (m)', 'Depth', 'Spacing', 'Burden']],
        body: patternData.drillPoints.map(hole => [
          hole.id,
          hole.x.toFixed(2),
          hole.y.toFixed(2),
          hole.depth?.toString() || '',
          hole.spacing?.toString() || '',
          hole.burden?.toString() || ''
        ]),
        theme: 'grid',
        headStyles: { fillColor: [44, 62, 80], textColor: 255, fontStyle: 'bold' },
        bodyStyles: { textColor: 44 },
        margin: { left: 50, right: 30 },
        styles: { fontSize: 9, cellPadding: 2 }
      });
      y = (doc as any).lastAutoTable.finalY + 20;
    }
    // Connections Section
    doc.setFontSize(15);
    doc.setTextColor(52, 73, 94);
    doc.text('Connections', 40, y);
    doc.line(40, y + 3, 200, y + 3);
    y += 15;
    autoTable(doc, {
      startY: y,
      head: [['From', 'To', 'Type', 'Delay (ms)', 'Sequence']],
      body: connections.map(conn => [
        conn.point1DrillPointId,
        conn.point2DrillPointId,
        conn.connectorType,
        conn.delay,
        conn.sequence
      ]),
      theme: 'grid',
      headStyles: { fillColor: [44, 62, 80], textColor: 255, fontStyle: 'bold' },
      bodyStyles: { textColor: 44 },
      margin: { left: 50, right: 30 },
      styles: { fontSize: 9, cellPadding: 2 }
    });
    y = (doc as any).lastAutoTable.finalY + 20;
    // Simulation Metrics Section
    doc.setFontSize(15);
    doc.setTextColor(52, 73, 94);
    doc.text('Simulation Metrics', 40, y);
    doc.line(40, y + 3, 200, y + 3);
    y += 15;
    const metricsToShow = [
      { label: 'Total Time (ms)', value: metrics['totalBlastTime'] },
      { label: 'Holes', value: metrics['holes'] },
      { label: 'Connections', value: metrics['connections'] }
    ];
    doc.setFontSize(12);
    metricsToShow.forEach((m, i) => {
      doc.text(`${m.label}: ${m.value}`, 50, y + i * 13);
    });
    y += metricsToShow.length * 13 + 10;
    // Pattern Visualization Section
    doc.setFontSize(15);
    doc.setTextColor(52, 73, 94);
    // Estimate the height needed for heading + image
    const headingHeight = 20;
    const imgWidth = 500;
    const imgHeightEstimate = 300; // Estimate, will be updated after rendering
    const pageHeight = doc.internal.pageSize.getHeight();
    // If not enough space for heading + image, add a new page
    if (y + headingHeight + imgHeightEstimate > pageHeight - 60) {
      doc.addPage();
      y = 40;
    }
    doc.text('Pattern Visualization', 40, y);
    doc.line(40, y + 3, 250, y + 3);
    y += 15;
    // Add canvas snapshot
    await html2canvas(canvasRef.nativeElement, {
      scale: 2,
      useCORS: true,
      allowTaint: true,
      backgroundColor: '#ffffff',
      logging: false,
      onclone: (clonedDoc) => {
        const clonedCanvas = clonedDoc.querySelector('.canvas-container');
        // Optionally ensure all holes are visible
      }
    }).then(canvas => {
      const imgData = canvas.toDataURL('image/png');
      const imgHeight = (canvas.height * imgWidth) / canvas.width;
      // If not enough space for the image, add a new page and re-add heading
      if (y + imgHeight > pageHeight - 60) {
        doc.addPage();
        y = 40;
        doc.setFontSize(15);
        doc.setTextColor(52, 73, 94);
        doc.text('Pattern Visualization', 40, y);
        doc.line(40, y + 3, 250, y + 3);
        y += 15;
      }
      doc.addImage(imgData, 'PNG', 40, y, imgWidth, imgHeight);
      // Footer
      const pageCount = (doc as any).getNumberOfPages();
      for (let i = 1; i <= pageCount; i++) {
        doc.setPage(i);
        doc.setFontSize(9);
        doc.setTextColor(150);
        doc.text(`Page ${i} of ${pageCount}`, doc.internal.pageSize.getWidth() - 80, doc.internal.pageSize.getHeight() - 20);
        doc.text(`Exported: ${new Date().toLocaleString()}`, 40, doc.internal.pageSize.getHeight() - 20);
      }
      doc.save(`blast-sequence-report-${new Date().toISOString().slice(0, 10)}.pdf`);
    });
  }
} 