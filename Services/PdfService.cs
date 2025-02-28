using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LMSAssignmentAPI.Models;


namespace LMSAssignmentAPI.Services
{
    public class PdfService
    {
        public string GenerateAssignmentPdf(List<Question> questions, string module)
        {
          
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"assignment_{timestamp}.pdf";

            
            string folderPath = "Uploads";

           
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

          
            string filePath = Path.Combine(folderPath, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

               
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Font questionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                Font choiceFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                pdfDoc.Add(new Paragraph($"Assignment - {module}", titleFont));
                pdfDoc.Add(new Paragraph("\n"));

               
                int qNumber = 1;
                foreach (var question in questions)
                {
                    pdfDoc.Add(new Paragraph($"{qNumber}. {question.QuestionText}", questionFont));
                    pdfDoc.Add(new Paragraph("\n"));

                   
                    if (question.Choices != null && question.Choices.Count == 4)
                    {
                        pdfDoc.Add(new Paragraph($"   A) {question.Choices[0]}", choiceFont));
                        pdfDoc.Add(new Paragraph($"   B) {question.Choices[1]}", choiceFont));
                        pdfDoc.Add(new Paragraph($"   C) {question.Choices[2]}", choiceFont));
                        pdfDoc.Add(new Paragraph($"   D) {question.Choices[3]}", choiceFont));
                    }

                    pdfDoc.Add(new Paragraph("\n"));
                    qNumber++;
                }

                pdfDoc.Close();
            }

            return filePath;
        }
    }
}
