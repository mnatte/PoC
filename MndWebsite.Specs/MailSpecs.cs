using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using System.Net.Mail;
using DecisionTree;
using MndWebsite.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MndWebsite.Controllers;
using MndWebsite.Contexts.ContactForm;

namespace MndWebsite.Specs
{

    public class MailSpecs
    {
        [Subject("Mail")]
        public class when_a_client_presses_the_send_button
        {
            static int ten;
            Establish context = () =>
            {
                ten = 5;
            };

            Because of = () =>
                ten += 5;

            It an_email_must_be_sent = () =>
                Assert.AreEqual(ten, 10);

        }

        [Subject("DecisionTree")]
        public class when_email_is_invalid_format
        {
            static BranchDecision<SubmitModel, TreeResult> tree;
            static ContactForm mail;
            
            Establish context = () =>
            {
                tree = 
                new BranchDecision<SubmitModel, TreeResult>(
                    "Email check",
                    (input) => input.EmailAddress.Contains('@'),
                    new BranchDecision<SubmitModel, TreeResult>(
                        "Subject not empty check",
                        (input) => input.Subject.Length > 0,
                        new MailResultDecision(true) { StatusCode = 123, Message = "Geslaagd!" },
                        new MailResultDecision(false) { StatusCode = 456, Message = "Subject empty!" }
                    ),
                    new MailResultDecision(false) { StatusCode = 999, Message = "Email faalde al!" }
                );
            };

            Because of = () =>
            {
                mail = new ContactFormData { EmailAddress = "wrongmail"};
            };

            It decision_must_fail = () =>
            {
                Assert.IsFalse(tree.Evaluate((SubmitModel)mail).Succeeded);
            };
        }

        [Subject("DecisionTree")]
        public class when_email_is_valid_and_subject_is_empty
        {
            static BranchDecision<SubmitModel, TreeResult> tree;
            static ContactForm mail;

            Establish context = () =>
            {
                tree =
                new BranchDecision<SubmitModel, TreeResult>(
                    "Email check",
                    (input) => input.EmailAddress.Contains('@'),
                    new BranchDecision<SubmitModel, TreeResult>(
                        "Subject not empty check",
                        (input) => input.Subject.Length > 0,
                        new MailResultDecision(true) { StatusCode = 123, Message = "Geslaagd!" },
                        new MailResultDecision(false) { StatusCode = 456, Message = "Subject empty!" }
                    ),
                    new MailResultDecision(false) { StatusCode = 999, Message = "Email faalde al!" }
                );
            };

            Because of = () =>
            {
                mail = new ContactFormData { EmailAddress = "test@test.nl", Subject = string.Empty };
            };

            It decision_must_fail = () =>
            {
                Assert.IsFalse(tree.Evaluate((SubmitModel)mail).Succeeded);
            };
        }

        public class when_email_is_valid_and_subject_is_not_empty_and_body_is_empty
        {
            static BranchDecision<SubmitModel, TreeResult> tree;
            static ContactForm mail;

            Establish context = () =>
            {
                tree =
                new BranchDecision<SubmitModel, TreeResult>(
                    "Email check",
                    (input) => input.EmailAddress.Contains('@'),
                    new BranchDecision<SubmitModel, TreeResult>(
                        "Subject not empty check",
                        (input) => !string.IsNullOrEmpty(input.Subject),
                        new BranchDecision<SubmitModel, TreeResult>(
                            "Body not empty check",
                            (input) => !string.IsNullOrEmpty(input.Body),
                            new MailResultDecision(true) { StatusCode = 123, Message = "Geslaagd!" },
                            new MailResultDecision(false) { StatusCode = 456, Message = "Body is empty!" }
                        ),
                        new MailResultDecision(false) { StatusCode = 456, Message = "Subject empty!" }
                    ),
                    new MailResultDecision(false) { StatusCode = 999, Message = "Email faalde al!" }
                );
            };

            Because of = () =>
            {
                mail = new ContactFormData { EmailAddress = "test@test.nl", Subject = "hallo", Body = string.Empty };
            };

            It decision_must_fail = () =>
            {
                Assert.IsFalse(tree.Evaluate((SubmitModel)mail).Succeeded);
            };
        }
    }
}
