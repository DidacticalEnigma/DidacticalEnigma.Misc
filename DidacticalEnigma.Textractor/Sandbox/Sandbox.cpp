#define _CRT_SECURE_NO_WARNINGS
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <cstdint>
#include <string>
#include <iostream>
#include "httplib.h"

void AppendJsonEscaped(std::string& out, const std::wstring& input)
{
	char a[128];
	for (auto ch : input)
	{
		sprintf(a, "\\u%04x", (unsigned int)ch);
		out += a;
	}
}

bool ProcessSentence(std::wstring& sentence)
{
	try
	{
		std::string address = "http://192.168.1.201:7000";
		httplib::Client cli(address);

		std::string body;
		body += R"({ "input": ")";
		AppendJsonEscaped(body, sentence);
		body += R"(" })";

		auto response = cli.Post("/session/inputReceive", body, "application/json");

		if (response)
		{
			auto status = response->status;
			if (status == 200)
			{
				return false;
			}
			else
			{
				sentence += L"\nERROR: Unexpected response ";
				sentence += std::wstring(address.begin(), address.end());
				sentence += L"  Status code ";
				sentence += std::to_wstring(status);
				return true;
			}
		}
		else
		{
			sentence += L"\nERROR: Connection error: ";

			switch (response.error())
			{
			case httplib::Error::Connection:
				sentence += L"Connection";
				break;
			case httplib::Error::BindIPAddress:
				sentence += L"BindIPAddress";
				break;
			case httplib::Error::Read:
				sentence += L"Read";
				break;
			case httplib::Error::Write:
				sentence += L"Write";
				break;
			case httplib::Error::ExceedRedirectCount:
				sentence += L"ExceedRedirectCount";
				break;
			case httplib::Error::Canceled:
				sentence += L"Canceled";
				break;
			case httplib::Error::SSLConnection:
				sentence += L"SSLConnection";
				break;
			case httplib::Error::SSLLoadingCerts:
				sentence += L"SSLLoadingCerts";
				break;
			case httplib::Error::SSLServerVerification:
				sentence += L"SSLServerVerification";
				break;
			case httplib::Error::UnsupportedMultipartBoundaryChars:
				sentence += L"UnsupportedMultipartBoundaryChars";
				break;
			case httplib::Error::Compression:
				sentence += L"Compression";
				break;
			case httplib::Error::ConnectionTimeout:
				sentence += L"ConnectionTimeout";
				break;
			case httplib::Error::ProxyConnection:
				sentence += L"ProxyConnection";
				break;
			case httplib::Error::Unknown:
				sentence += L"Unknown";
				break;
			}
			return true;
		}
	}
	catch (...)
	{
		sentence += L"\nERROR: Unknown error";
		return true;
	}
}


int main()
{
	std::wstring a = L"dsrfjowjfsfjsf";
	ProcessSentence(a);
}
